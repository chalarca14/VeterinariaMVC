using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MVC.Data.Data;
using MVC.Data.DTO.Mascotas;
using MVC.Data.Models;
using MVC.Domain.Services.Interfaces;

namespace MVC.Domain.Services
{
    public class MascotaServices : IMascotaServices
    {
        #region Properties & Constructor
        private readonly VeterinariaContext _context;
        private readonly IWebHostEnvironment _environment;

        public MascotaServices(VeterinariaContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        #endregion

        #region Methods
        public async Task<List<MascotaDto>> GetAllMascotasAsync()
        {
            var list = await _context.Mascotas
                .Include(m => m.IdPropietarioNavigation)
                .Include(m => m.IdRazaNavigation)
                    .ThenInclude(r => r.IdEspecieNavigation)
                .ToListAsync();

            return list.Select(x => new MascotaDto
            {
                IdMascota = x.Id,
                Nombre = x.Nombre,
                FechaNacimiento = x.FechaNacimiento,
                Peso = x.Peso,
                RutaFoto = x.RutaFoto,
                IdPropietario = x.IdPropietario,
                NombrePropietario = $"{x.IdPropietarioNavigation?.Nombre} {x.IdPropietarioNavigation?.Apellido}",
                IdRaza = x.IdRaza,
                NombreRaza = x.IdRazaNavigation?.Nombre ?? "",
                IdEspecie = x.IdRazaNavigation?.IdEspecie ?? 0,
                NombreEspecie = x.IdRazaNavigation?.IdEspecieNavigation?.Nombre ?? ""
            }).ToList();
        }

        public async Task<bool> AddMascotaAsync(AddMascotaDto add)
        {
            string? rutaFoto = await GuardarImagenAsync(add.FotoFile);

            var entity = new Mascota
            {
                Nombre = add.Nombre,
                FechaNacimiento = add.FechaNacimiento,
                Peso = add.Peso,
                IdPropietario = add.IdPropietario,
                IdRaza = add.IdRaza,
                RutaFoto = rutaFoto
            };

            _context.Mascotas.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateMascotaAsync(UpdateMascotaDto update)
        {
            var entity = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == update.IdMascota);
            if (entity == null) throw new Exception($"La mascota con ID {update.IdMascota} no existe.");

            if (update.FotoFile != null && update.FotoFile.Length > 0)
            {
                EliminarImagenExistente(entity.RutaFoto);
                entity.RutaFoto = await GuardarImagenAsync(update.FotoFile);
            }

            entity.Nombre = update.Nombre;
            entity.FechaNacimiento = update.FechaNacimiento;
            entity.Peso = update.Peso;
            entity.IdPropietario = update.IdPropietario;
            entity.IdRaza = update.IdRaza;

            _context.Mascotas.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteMascotaAsync(int id)
        {
            var entity = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) throw new Exception($"La mascota con ID {id} no existe.");

            EliminarImagenExistente(entity.RutaFoto);

            _context.Mascotas.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }
        #endregion

        #region Helpers para Archivos y Validación
        private void ValidarImagen(IFormFile file)
        {
            long maxBytes = 5 * 1024 * 1024; // 5 MB máximo
            if (file.Length > maxBytes)
            {
                throw new Exception($"El archivo {file.FileName} supera el peso máximo de 5 MB.");
            }

            string[] extensionesPermitidas = { ".jpg", ".jpeg", ".png", ".webp" };
            string extensionActual = Path.GetExtension(file.FileName).ToLower();

            if (!extensionesPermitidas.Contains(extensionActual))
            {
                throw new Exception($"La extensión {extensionActual} no es permitida. Solo se admiten imágenes (.jpg, .jpeg, .png, .webp).");
            }
        }

        private async Task<string?> GuardarImagenAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            ValidarImagen(file);

            string folderPath = Path.Combine(_environment.WebRootPath, "images", "mascotas");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/mascotas/{fileName}";
        }

        private void EliminarImagenExistente(string? rutaFoto)
        {
            if (string.IsNullOrEmpty(rutaFoto)) return;

            string localPath = Path.Combine(_environment.WebRootPath, rutaFoto.TrimStart('/'));
            if (File.Exists(localPath))
            {
                File.Delete(localPath);
            }
        }
        #endregion
    }
}