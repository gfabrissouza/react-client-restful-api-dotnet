using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiDotNet.Business;
using RestApiDotNet.Data.VO;
using RestApiDotNet.HyperMedia.Filters;

namespace RestApiDotNet.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;
        private IPersonBusiness _personBusiness;

        public PersonController(ILogger<PersonController> logger, IPersonBusiness personBusiness)
        {
            _logger = logger;
            _personBusiness = personBusiness;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(400, Type = typeof(PersonVO))]
        [ProducesResponseType(401, Type = typeof(PersonVO))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(List<PersonVO>))]
        [ProducesResponseType(400, Type = typeof(List<PersonVO>))]
        [ProducesResponseType(401, Type = typeof(List<PersonVO>))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var person = _personBusiness.FindByID(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpGet("find-person-by-name")]
        [ProducesResponseType(200, Type = typeof(List<PersonVO>))]
        [ProducesResponseType(400, Type = typeof(List<PersonVO>))]
        [ProducesResponseType(401, Type = typeof(List<PersonVO>))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get([FromQuery] string? firstName, string? lastName)
        {
            var person = _personBusiness.FindByName(firstName, lastName);
            if (person == null) return NotFound();
            return Ok(person);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(200, Type = typeof(List<PersonVO>))]
        [ProducesResponseType(400, Type = typeof(List<PersonVO>))]
        [ProducesResponseType(401, Type = typeof(List<PersonVO>))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Patch(long id)
        {
            var person = _personBusiness.Disable(id);
            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(400, Type = typeof(PersonVO))]
        [ProducesResponseType(401, Type = typeof(PersonVO))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] PersonVO personVO)
        {
            if (personVO == null) return BadRequest();
            return Ok(_personBusiness.Create(personVO));
        }
        
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(PersonVO))]
        [ProducesResponseType(400, Type = typeof(PersonVO))]
        [ProducesResponseType(401, Type = typeof(PersonVO))]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] PersonVO personVO)
        {
            if (personVO == null) return BadRequest();
            var updatedPerson = _personBusiness.Update(personVO);
            if (updatedPerson == null) return NotFound();
            return Ok(updatedPerson);
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(204, Type = typeof(PersonVO))]
        [ProducesResponseType(400, Type = typeof(PersonVO))]
        [ProducesResponseType(401, Type = typeof(PersonVO))]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }
    }
}
