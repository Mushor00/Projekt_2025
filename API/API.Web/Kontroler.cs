using Microsoft.AspNetCore.Mvc;
using API.ApiService;

namespace API.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        public ReservationController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("rezerwuj")]
        public async Task<IActionResult> RezerwujSale([FromBody] RezerwacjaRequest request)
        {
            var sukces = await _reservationService.ZarezerwujSaleAsync(
                request.NumerSali,
                request.Login,
                request.DataOd,
                request.DataDo
            );

            if (sukces)
                return Ok(new { message = "Sala zarezerwowana pomyślnie!" });
            else
                return Conflict(new { message = "Sala jest już zajęta w tym terminie." });
        }
        [HttpDelete("usun/{idRezerwacji}")]
        public async Task<IActionResult> UsunRezerwacje(int idRezerwacji)
        {
            var sukces = await _reservationService.UsunRezerwacjeAsync(idRezerwacji);

            if (sukces)
                return Ok(new { message = "Rezerwacja usunięta." });
            else
                return NotFound(new { message = "Nie znaleziono rezerwacji o podanym ID." });
        }
        [HttpPut("edytuj/{idRezerwacji}")]
        public async Task<IActionResult> EdytujRezerwacje(int idRezerwacji, [FromBody] EdytujRezerwacjeRequest request)
        {
            var sukces = await _reservationService.EdytujRezerwacjeAsync(idRezerwacji, request.NowaDataOd, request.NowaDataDo);

            if (sukces)
                return Ok(new { message = "Rezerwacja została zaktualizowana." });
            else
                return Conflict(new { message = "Nie udało się zaktualizować rezerwacji." });
        }

        public class EdytujRezerwacjeRequest
        {
            public DateTime NowaDataOd { get; set; }
            public DateTime NowaDataDo { get; set; }
        }

    }

    // Dane przychodzące z frontu?
    public class RezerwacjaRequest
    {
        public int NumerSali { get; set; }
        public string Login { get; set; } = string.Empty;
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }
    }
}
