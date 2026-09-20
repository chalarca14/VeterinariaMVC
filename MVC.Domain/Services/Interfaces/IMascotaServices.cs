using MVC.Data.DTO.Mascotas;

namespace MVC.Domain.Services.Interfaces
{
    public interface IMascotaServices
    {
        Task<List<MascotaDto>> GetAllMascotasAsync();
        Task<bool> AddMascotaAsync(AddMascotaDto add);
        Task<bool> UpdateMascotaAsync(UpdateMascotaDto update);
        Task<bool> DeleteMascotaAsync(int id);
    }
}