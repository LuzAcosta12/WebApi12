﻿using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using WebApi29.Services.IServices;
using WebApi29.Services.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApi29.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class RolController : ControllerBase
    {
        private readonly IRolServices _rolServices;
        public RolController(IRolServices rolServices)
        {
            _rolServices = rolServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _rolServices.GetAllRoles();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ById(int id)
        {
            return Ok(await _rolServices.RolesById(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Crear(RolRequest request)
        {
            var response = await _rolServices.Crear(request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RolRequest request)
        {
            var response = await _rolServices.Actualizar(id, request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var respoonse = await _rolServices.Eliminar(id);
            return Ok(respoonse);
        }
    }

}