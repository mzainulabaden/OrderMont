using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ERP.Generics
{
    [AbpAuthorize]
    public abstract class GenericAppService<MainDto, GetSingleDto, GetAllDto, BulkDto, TEntity> : ApplicationService where MainDto : EntityDto<long> where GetSingleDto : EntityDto<long> where GetAllDto : EntityDto<long> where BulkDto : EntityDto<long> where TEntity : ERPProjBaseEntity
    {
        public IRepository<TEntity, long> MainRepository { get; set; }

        public GenericAppService()
        {
            CheckForPermissions = true;
        }

        public async virtual Task<MainDto> Create(MainDto input)
        {
            CheckCreatePermission();

            var errors = "";
            var id_errors = CheckIDs(input);
            if (id_errors != null && id_errors.Count > 0)
                errors += id_errors.ListStringToString('\n');
            var duplication_errors = CheckDbDuplication(input, true, null);
            if (duplication_errors != null && duplication_errors.Count > 0)
                errors += duplication_errors.ListStringToString('\n');
            if (errors != "")
                throw new UserFriendlyException(errors);

            var output = ObjectMapper.Map<TEntity>(input);
            output.TenantId = AbpSession.TenantId;

            var response = PreOps(output, DB_OPERATION.CREATE, null);
            if (response.Item2.Count == 0)
            {
                using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                {
                    output = response.Item1;
                    await MainRepository.InsertAsync(output);
                    CurrentUnitOfWork.SaveChanges();

                    var extra_operation_response = PostOps(output, DB_OPERATION.CREATE);
                    if (extra_operation_response != null)
                        throw new UserFriendlyException(extra_operation_response);

                    unitOfWork.Complete();
                }

                return ObjectMapper.Map<MainDto>(output);
            }
            else
            {
                errors = "";
                foreach (var item in response.Item2)
                    errors += $"{item}\n";
                throw new UserFriendlyException(errors);
            }
        }

        public virtual GetSingleDto Get(long Id)
        {
            var from_database = GetByID(Id);
            if (from_database != null)
            {

                return from_database;
            }
            else
                throw new UserFriendlyException($"Could not find {GetName()} with ID: '{Id}'.");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual PagedResultDto<GetAllDto> GetAllSimple(PagedResultRequestDto input)
        {
            var from_database = MainRepository.GetAll().Where(i => i.TenantId == AbpSession.TenantId);
            from_database = from_database.OrderByDescending(i => i.CreationTime);
            var paged = from_database.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var result = new PagedResultDto<GetAllDto>(from_database.Count(), ObjectMapper.Map<List<GetAllDto>>(paged));
            return result;
        }

        public async virtual Task<MainDto> Update(MainDto input)
        {
            CheckUpdatePermission();

            var entity = MainRepository.GetAll().Where(i => i.Id == input.Id && i.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (entity == null)
                throw new UserFriendlyException($"Could not find {GetName()} with ID: '{input.Id}'.");

            var errors = "";
            var id_errors = CheckIDs(input);
            if (id_errors != null && id_errors.Count > 0)
                errors += id_errors.ListStringToString('\n');
            var duplication_errors = CheckDbDuplication(input, true, null);
            if (duplication_errors != null && duplication_errors.Count > 0)
                errors += duplication_errors.ListStringToString('\n');
            if (errors != "")
                throw new UserFriendlyException(errors);

            var fetch_project = MainRepository.GetAll().Where(i => i.Id == input.Id && i.TenantId == AbpSession.TenantId).FirstOrDefault();
            var new_entity = ObjectMapper.Map<TEntity>(fetch_project);
            var mapper = new MapperConfiguration(i => i.CreateMap(typeof(MainDto), typeof(TEntity))).CreateMapper();
            new_entity = mapper.Map(input, new_entity);

            var response = PreOps(new_entity, DB_OPERATION.UPDATE, null);
            if (response.Item2 == null || response.Item2.Count == 0)
            {
                using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                {
                    new_entity = response.Item1;
                    await MainRepository.UpdateAsync(new_entity);
                    CurrentUnitOfWork.SaveChanges();

                    var extra_operation_response = PostOps(new_entity, DB_OPERATION.UPDATE);
                    if (extra_operation_response != null)
                        throw new UserFriendlyException(extra_operation_response);

                    unitOfWork.Complete();
                }

                return ObjectMapper.Map<MainDto>(new_entity);
            }
            else
            {
                errors = "";
                foreach (var item in response.Item2)
                    errors += $"{item}\n";
                throw new UserFriendlyException(errors);
            }
        }

        public async virtual Task<string> Delete(EntityDto<long> input)
        {
            CheckDeletePermission();

            var result = MainRepository.GetAll().Where(x => x.Id == input.Id && x.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (result != null)
            {
                var error = CheckAllocations(result);
                if (error != null)
                    throw new UserFriendlyException(error);
                else
                {
                    using (var unitOfWork = UnitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                    {
                        await MainRepository.DeleteAsync(result);
                        CurrentUnitOfWork.SaveChanges();

                        var extra_operation_response = PostOps(result, DB_OPERATION.DELETE);
                        if (extra_operation_response != null)
                            throw new UserFriendlyException(extra_operation_response);

                        unitOfWork.Complete();
                    }
                }
            }
            else throw new UserFriendlyException($"Could not find {GetName()} with ID: '{input.Id}'.");

            return "Deleted Successfully.";
        }

        /* -------------------------------------------------------------------------------------------- */

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual List<string> CheckInFileDuplication(Dictionary<int, TEntity> dict)
        {
            var output = new List<string>();
            if (dict.Count == 0)
                return output;

            if (InFileDuplicates(dict.ElementAt(0)) == null)
                return output;

            var groups = dict.GroupBy(i => InFileDuplicates(i));
            foreach (var group in groups)
                if (group.Count() > 1)
                {
                    output.Add($"Found {group.Count()} duplicates listed below:");
                    foreach (var unit in group)
                        output.Add($"Duplicate Data at Row: '{unit.Key}'.");
                }

            return output;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual GetSingleDto GetByID(long id)
        {
            var entity = MainRepository.GetAll().Where(i => i.Id == id && i.TenantId == AbpSession.TenantId).FirstOrDefault();
            if (entity != null)
                return ObjectMapper.Map<GetSingleDto>(entity);
            else return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual GetSingleDto NamesToIDs(BulkDto input, int index, ref List<string> errorlist)
        {
            return ObjectMapper.Map<GetSingleDto>(input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual Tuple<TEntity, List<string>> PreOps(TEntity input, DB_OPERATION operation, int? index)
        {
            return new Tuple<TEntity, List<string>>(input, new List<string>());
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual string PostOps(TEntity entity, DB_OPERATION operation)
        {
            return null;
        }

        /* -------------------------------------------------------------------------------------------- */

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual List<string> CheckIDs(MainDto input)
        {
            return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual List<string> CheckDbDuplication(MainDto input, bool updating, int? index)
        {
            return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual dynamic InFileDuplicates(KeyValuePair<int, TEntity> dict) => null;

        [ApiExplorerSettings(IgnoreApi = true)]
        public virtual string CheckAllocations(TEntity input)
        {
            return null;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetName() => typeof(TEntity).Name.Replace("Info", "");

        /* -------------------------------------------------------------------------------------------- */

        public class BulkListDto
        {
            public List<BulkDto> BulkDtos { get; set; }
        }

        public enum DB_OPERATION
        {
            CREATE, UPDATE, DELETE
        }

        /* -------------------------------------------------------------------------------------------- */

        [ApiExplorerSettings(IgnoreApi = true)]
        public void CheckCreatePermission()
        {
            if (CheckForPermissions && !PermissionChecker.IsGranted(CREATE_PERMISSION))
                throw new UserFriendlyException($"Don't have permission to CREATE '{GetName()}'.");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void CheckUpdatePermission()
        {
            if (CheckForPermissions && !PermissionChecker.IsGranted(UPDATE_PERMISSION))
                throw new UserFriendlyException($"Don't have permission to Update '{GetName()}'.");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void CheckDeletePermission()
        {
            if (CheckForPermissions && !PermissionChecker.IsGranted(DELETE_PERMISSION))
                throw new UserFriendlyException($"Don't have permission to DELETE '{GetName()}'.");
        }

        public virtual string CREATE_PERMISSION => string.Format(ERPConsts.CREATE_PERMISSION, GetName());
        public virtual string UPDATE_PERMISSION => string.Format(ERPConsts.UPDATE_PERMISSION, GetName());
        public virtual string DELETE_PERMISSION => string.Format(ERPConsts.DELETE_PERMISSION, GetName());
        public bool CheckForPermissions { get; set; }
    }
}
