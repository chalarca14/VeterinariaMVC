using Microsoft.EntityFrameworkCore;
using MVC.Data.Data;
using MVC.Data.DTO.Propietarios;
using MVC.Data.Models;
using MVC.Domain.Services.Interfaces;

namespace MVC.Domain.Services
{
    public class PropietarioServices : IPropietarioServices
    {
        #region Properties
        private readonly VeterinariaContext _context;
        #endregion

        #region Constructor
        public PropietarioServices(VeterinariaContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public async Task<List<PropietarioDto>> GetAllPropietarios()
        {
            List<Propietario> list = await _context.Propietarios.ToListAsync();
            List<PropietarioDto> result = list.Select(x => new PropietarioDto()
            {
                IdPropietario = x.Id, // Se mapea Id de la entidad a IdPropietario del DTO
                Nombre = x.Nombre,
                Apellido = x.Apellido,
                Telefono = x.Telefono,
                Email = x.Email,
                Direccion = x.Direccion
            }).ToList();

            return result;
        }

        public async Task<bool> AddPropietarioAsync(AddPropietarioDto add)
        {
            Propietario entity = new Propietario()
            {
                Nombre = add.Nombre,
                Apellido = add.Apellido,
                Telefono = add.Telefono,
                Email = add.Email,
                Direccion = add.Direccion
            };

            _context.Propietarios.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePropietarioAsync(UpdatePropietarioDto update)
        {
            Propietario entity = await GetPropietarioAsync(update.IdPropietario);
            entity.Nombre = update.Nombre;
            entity.Apellido = update.Apellido;
            entity.Telefono = update.Telefono;
            entity.Email = update.Email;
            entity.Direccion = update.Direccion;

            _context.Propietarios.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePropietarioAsync(int id)
        {
            try
            {
                Propietario entity = await GetPropietarioAsync(id);
                _context.Propietarios.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Privates
        private async Task<Propietario> GetPropietarioAsync(int id)
        {
            var entity = await _context.Propietarios.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new Exception($"El Propietario con el ID {id} no existe.");
            }
            return entity;
        }
        #endregion
    }
}