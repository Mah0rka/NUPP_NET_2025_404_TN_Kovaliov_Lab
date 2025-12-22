using Fish.Common.Services;
using Fish.Infrastructure.Models;
using Fish.REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fish.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AquariumsController : ControllerBase
    {
        private readonly ICrudServiceAsync<AquariumModel> _aquariumService;

        public AquariumsController(ICrudServiceAsync<AquariumModel> aquariumService)
        {
            _aquariumService = aquariumService;
        }

        // GET: api/aquariums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AquariumResponseDto>>> GetAll([FromQuery] int? page, [FromQuery] int? amount)
        {
            try
            {
                IEnumerable<AquariumModel> aquariums;

                // Якщо є параметри пагінації
                if (page.HasValue && amount.HasValue)
                {
                    aquariums = await _aquariumService.ReadAllAsync(page.Value, amount.Value);
                }
                else
                {
                    aquariums = await _aquariumService.ReadAllAsync();
                }

                var response = aquariums.Select(a => new AquariumResponseDto
                {
                    Id = a.ExternalId,
                    Name = a.Name,
                    Volume = a.Volume,
                    Location = a.Location,
                    FishCount = a.Fishes?.Count ?? 0
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // GET: api/aquariums/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AquariumResponseDto>> GetById(Guid id)
        {
            try
            {
                var aquarium = await _aquariumService.ReadAsync(id);

                if (aquarium == null)
                    return NotFound($"Акваріум з ID {id} не знайдено");

                var response = new AquariumResponseDto
                {
                    Id = aquarium.ExternalId,
                    Name = aquarium.Name,
                    Volume = aquarium.Volume,
                    Location = aquarium.Location,
                    FishCount = aquarium.Fishes?.Count ?? 0
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // POST: api/aquariums
        [HttpPost]
        public async Task<ActionResult<AquariumResponseDto>> Create([FromBody] AquariumDto aquariumDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var aquarium = new AquariumModel
                {
                    ExternalId = Guid.NewGuid(),
                    Name = aquariumDto.Name,
                    Volume = aquariumDto.Volume,
                    Location = aquariumDto.Location
                };

                var created = await _aquariumService.CreateAsync(aquarium);

                if (!created)
                    return StatusCode(500, "Не вдалося створити акваріум");

                var response = new AquariumResponseDto
                {
                    Id = aquarium.ExternalId,
                    Name = aquarium.Name,
                    Volume = aquarium.Volume,
                    Location = aquarium.Location,
                    FishCount = 0
                };

                return CreatedAtAction(nameof(GetById), new { id = aquarium.ExternalId }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // PUT: api/aquariums/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] AquariumDto aquariumDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingAquarium = await _aquariumService.ReadAsync(id);

                if (existingAquarium == null)
                    return NotFound($"Акваріум з ID {id} не знайдено");

                // Оновлюємо властивості
                existingAquarium.Name = aquariumDto.Name;
                existingAquarium.Volume = aquariumDto.Volume;
                existingAquarium.Location = aquariumDto.Location;

                var updated = await _aquariumService.UpdateAsync(existingAquarium);

                if (!updated)
                    return StatusCode(500, "Не вдалося оновити акваріум");

                return Ok("Акваріум успішно оновлено");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // DELETE: api/aquariums/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var aquarium = await _aquariumService.ReadAsync(id);

                if (aquarium == null)
                    return NotFound($"Акваріум з ID {id} не знайдено");

                var deleted = await _aquariumService.RemoveAsync(aquarium);

                if (!deleted)
                    return StatusCode(500, "Не вдалося видалити акваріум");

                return Ok("Акваріум успішно видалено");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
    }
}

