using Fish.Common.Services;
using Fish.Infrastructure.Models;
using Fish.REST.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fish.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FishesController : ControllerBase
    {
        private readonly ICrudServiceAsync<FishModel> _fishService;

        public FishesController(ICrudServiceAsync<FishModel> fishService)
        {
            _fishService = fishService;
        }

        // GET: api/fishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FishResponseDto>>> GetAll([FromQuery] int? page, [FromQuery] int? amount)
        {
            try
            {
                IEnumerable<FishModel> fishes;

                // Якщо є параметри пагінації
                if (page.HasValue && amount.HasValue)
                {
                    fishes = await _fishService.ReadAllAsync(page.Value, amount.Value);
                }
                else
                {
                    fishes = await _fishService.ReadAllAsync();
                }

                var response = fishes.Select(f => new FishResponseDto
                {
                    Id = f.ExternalId,
                    Variety = f.Variety,
                    Habitat = f.Habitat,
                    TopSpeed = f.TopSpeed,
                    IsPredatory = f.IsPredatory,
                    Length = f.Length,
                    AquariumId = f.AquariumId,
                    AquariumName = f.Aquarium?.Name
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // GET: api/fishes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FishResponseDto>> GetById(Guid id)
        {
            try
            {
                var fish = await _fishService.ReadAsync(id);

                if (fish == null)
                    return NotFound($"Рибу з ID {id} не знайдено");

                var response = new FishResponseDto
                {
                    Id = fish.ExternalId,
                    Variety = fish.Variety,
                    Habitat = fish.Habitat,
                    TopSpeed = fish.TopSpeed,
                    IsPredatory = fish.IsPredatory,
                    Length = fish.Length,
                    AquariumId = fish.AquariumId,
                    AquariumName = fish.Aquarium?.Name
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // POST: api/fishes
        [HttpPost]
        public async Task<ActionResult<FishResponseDto>> Create([FromBody] FishDto fishDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var fish = new FishModel
                {
                    ExternalId = Guid.NewGuid(),
                    Variety = fishDto.Variety,
                    Habitat = fishDto.Habitat,
                    TopSpeed = fishDto.TopSpeed,
                    IsPredatory = fishDto.IsPredatory,
                    Length = fishDto.Length,
                    AquariumId = fishDto.AquariumId
                };

                var created = await _fishService.CreateAsync(fish);

                if (!created)
                    return StatusCode(500, "Не вдалося створити рибу");

                var response = new FishResponseDto
                {
                    Id = fish.ExternalId,
                    Variety = fish.Variety,
                    Habitat = fish.Habitat,
                    TopSpeed = fish.TopSpeed,
                    IsPredatory = fish.IsPredatory,
                    Length = fish.Length,
                    AquariumId = fish.AquariumId
                };

                return CreatedAtAction(nameof(GetById), new { id = fish.ExternalId }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // PUT: api/fishes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] FishDto fishDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingFish = await _fishService.ReadAsync(id);

                if (existingFish == null)
                    return NotFound($"Рибу з ID {id} не знайдено");

                // Оновлюємо властивості
                existingFish.Variety = fishDto.Variety;
                existingFish.Habitat = fishDto.Habitat;
                existingFish.TopSpeed = fishDto.TopSpeed;
                existingFish.IsPredatory = fishDto.IsPredatory;
                existingFish.Length = fishDto.Length;
                existingFish.AquariumId = fishDto.AquariumId;

                var updated = await _fishService.UpdateAsync(existingFish);

                if (!updated)
                    return StatusCode(500, "Не вдалося оновити рибу");

                return Ok("Рибу успішно оновлено");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }

        // DELETE: api/fishes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var fish = await _fishService.ReadAsync(id);

                if (fish == null)
                    return NotFound($"Рибу з ID {id} не знайдено");

                var deleted = await _fishService.RemoveAsync(fish);

                if (!deleted)
                    return StatusCode(500, "Не вдалося видалити рибу");

                return Ok("Рибу успішно видалено");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Помилка сервера: {ex.Message}");
            }
        }
    }
}

