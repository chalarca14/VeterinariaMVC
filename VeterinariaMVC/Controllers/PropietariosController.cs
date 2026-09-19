using Microsoft.AspNetCore.Mvc;
using MVC.Data.DTO.Propietarios;
using MVC.Domain.Services.Interfaces;

namespace VeterinariaMVC.Controllers
{
    public class PropietariosController : Controller
    {
        private readonly IPropietarioServices _propietarioServices;

        public PropietariosController(IPropietarioServices propietarioServices)
        {
            _propietarioServices = propietarioServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllPropietarios")]
        public async Task<IActionResult> GetAllPropietarios()
        {
            var entities = await _propietarioServices.GetAllPropietarios();
            return Ok(entities);
        }

        [HttpPost("AddPropietario")]
        public async Task<IActionResult> AddPropietario(AddPropietarioDto add)
        {
            bool success = await _propietarioServices.AddPropietarioAsync(add);
            return Ok(success);
        }

        [HttpPut("UpdatePropietario")]
        public async Task<IActionResult> UpdatePropietario(UpdatePropietarioDto update)
        {
            bool success = await _propietarioServices.UpdatePropietarioAsync(update);
            return Ok(success);
        }

        [HttpDelete("DeletePropietario")]
        public async Task<IActionResult> DeletePropietario(int id)
        {
            bool success = await _propietarioServices.DeletePropietarioAsync(id);
            return Ok(success);
        }
    }
}