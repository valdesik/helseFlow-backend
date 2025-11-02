using HelseFlow_Backend.Application.DTOs;

namespace HelseFlow_Backend.Api.Features.Doctors.GetAllDoctors;

public class GetAllDoctorsResponse
{
    public List<DoctorDto> Doctors { get; set; }
}
