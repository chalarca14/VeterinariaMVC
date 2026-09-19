using MVC.Data.DTO.Propietarios;

namespace MVC.Domain.Services.Interfaces
{
    public interface IPropietarioServices
    {
        Task<List<PropietarioDto>> GetAllPropietarios();
        Task<bool> AddPropietarioAsync(AddPropietarioDto add);
        Task<bool> UpdatePropietarioAsync(UpdatePropietarioDto update);
        Task<bool> DeletePropietarioAsync(int id);
    }
}