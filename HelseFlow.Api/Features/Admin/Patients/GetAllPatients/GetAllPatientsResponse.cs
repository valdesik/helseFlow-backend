using HelseFlow_Backend.Application.DTOs;

namespace HelseFlow_Backend.Api.Features.Admin.Patients.GetAllPatients;

public class GetAllPatientsResponse
{
    public List<UserDto> Patients { get; set; }
}
