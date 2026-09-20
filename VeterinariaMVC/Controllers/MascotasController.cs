using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Data.Data;
using MVC.Data.DTO.Mascotas;
using MVC.Domain.Services.Interfaces;

namespace VeterinariaMVC.Controllers
{
    public class MascotasController : Controller
    {
        #region Properties & Constructor
        private readonly IMascotaServices _mascotaServices;
        private readonly VeterinariaContext _context;

        public MascotasController(IMascotaServices mascotaServices, VeterinariaContext context)
        {
            _mascotaServices = mascotaServices;
            _context = context;
        }
        #endregion

        #region Views
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Endpoints CRUD
        [HttpGet("GetAllMascotas")]
        public async Task<IActionResult> GetAllMascotas()
        {
            var result = await _mascotaServices.GetAllMascotasAsync();
            return Ok(result);
        }

        [HttpPost("AddMascota")]
        public async Task<IActionResult> AddMascota([FromForm] AddMascotaDto add)
        {
            bool success = await _mascotaServices.AddMascotaAsync(add);
            return Ok(success);
        }

        [HttpPost("UpdateMascota")]
        public async Task<IActionResult> UpdateMascota([FromForm] UpdateMascotaDto update)
        {
            bool success = await _mascotaServices.UpdateMascotaAsync(update);
            return Ok(success);
        }

        [HttpDelete("DeleteMascota")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            bool success = await _mascotaServices.DeleteMascotaAsync(id);
            return Ok(success);
        }
        #endregion

        #region Endpoints para Combos en Cascada
        [HttpGet("GetPropietariosCombo")]
        public async Task<IActionResult> GetPropietariosCombo()
        {
            var list = await _context.Propietarios
                .Select(p => new { p.Id, NombreCompleto = $"{p.Nombre} {p.Apellido}" })
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("GetEspeciesCombo")]
        public async Task<IActionResult> GetEspeciesCombo()
        {
            var list = await _context.Especies
                .Select(e => new { e.Id, e.Nombre })
                .ToListAsync();
            return Ok(list);
        }

        [HttpGet("GetRazasPorEspecieCombo")]
        public async Task<IActionResult> GetRazasPorEspecieCombo(int idEspecie)
        {
            var list = await _context.Razas
                .Where(r => r.IdEspecie == idEspecie)
                .Select(r => new { r.Id, r.Nombre })
                .ToListAsync();
            return Ok(list);
        }
        #endregion
    }
}