using Genesis.WebApp.Models;

namespace Genesis.WebApp.Services
{
    public class RoleService
    {

        private readonly IApiService api;
        public RoleService(IJwtService _jwtService, IApiService _api, StateService _state)
        {
            api = _api;
        }
        public async Task<ApiResponse<Genesis.Shared.UserRoleManagements.RoleResponse>> GetRoles(string filterColumn, string filterQuery, string sortColumn, string sortOrder, int pageSize, int pageIndex)
        {
            var response = await api.GetAsync<Genesis.Shared.UserRoleManagements.RoleResponse>($"/Roles/GetRoles/", new Dictionary<string, string>
            {
                { "pageIndex", pageIndex.ToString() },
                { "pageSize", pageSize.ToString() },
                { "filterColumn", filterColumn },
                {"filterQuery",filterQuery },
                {"sortColumn",sortColumn },
                {"sortOrder",sortOrder }
            });
            return response;
        }

        public async Task<ApiResponse<Genesis.Shared.DTO.RoleDTO>> GetRole(int roleId)
        {
            var response = await api.GetAsync<Genesis.Shared.DTO.RoleDTO>($"/Roles/GetRoleId/", new Dictionary<string, string>
            {
                { "roleId", roleId.ToString() }
            });
            return response;
        }

        public async Task<ApiResponse<RoleResponse>> UpdateAsync(Genesis.Shared.DTO.RoleDTO credentials)
        {
            var response = await api.PutAsync<RoleResponse>("/Roles/UpdateRole", credentials);
            return response;
        }

        public async Task<ApiResponse<RoleResponse>> AddAsync(Genesis.Shared.DTO.RoleDTO credentials)
        {
            var response = await api.PostAsync<RoleResponse>("/Roles/AddRole", credentials);
            return response;
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string roleId)
        {
            var response = await api.DeleteAsync<bool>($"/Roles/DeleteRole/", new Dictionary<string, string>
            {
                { "id", roleId.ToString() }
            });
            return response;
        }
    }
    public class RoleResponse
    {
        public RoleModel Role { get; set; }
    }
}
