using Microsoft.AspNetCore.Mvc;
using p3mo_user_crud_backend.DTOs;
using p3mo_user_crud_backend.Services;

namespace p3mo_user_crud_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            IRoleService roleService,
            ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all roles");
                return StatusCode(500, "Internal server error occurred while retrieving roles");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound($"Role with ID {id} not found");
                }
                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting role with ID {RoleId}", id);
                return StatusCode(500, $"Internal server error occurred while retrieving role with ID {id}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole(RoleDto roleDto)
        {
            try
            {
                var createdRole = await _roleService.CreateRoleAsync(roleDto);
                return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new role");
                return StatusCode(500, "Internal server error occurred while creating a new role");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDto>> UpdateRole(int id, RoleDto roleDto)
        {
            try
            {
                var updatedRole = await _roleService.UpdateRoleAsync(id, roleDto);
                if (updatedRole == null)
                {
                    return NotFound($"Role with ID {id} not found");
                }
                return Ok(updatedRole);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating role with ID {RoleId}", id);
                return StatusCode(500, $"Internal server error occurred while updating role with ID {id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            try
            {
                var result = await _roleService.DeleteRoleAsync(id);
                if (!result)
                {
                    return BadRequest($"Role with ID {id} cannot be deleted because it is assigned to users or does not exist");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting role with ID {RoleId}", id);
                return StatusCode(500, $"Internal server error occurred while deleting role with ID {id}");
            }
        }
    }
} 