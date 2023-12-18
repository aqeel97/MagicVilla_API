using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MagicVilla_VillaAPI.Controllers
{
    //use Route to solve the routing issue {define routing}
    //[Route("api/[controller]")]
    [Route("api/VillaAPI")]
    //when commit the [ApiController] the program not check the rules as like Required or maximum linth in Models commit this and try to check
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        //use logger to right on CMD or Console
        private readonly ILogger<VillaAPIController> _logger;
        public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            _logger.LogInformation("Getting All Villas");
            return Ok(VillaStore.villalist);
        }

        [HttpGet("{id:int}",Name ="GetVilla")]//GetVilla is the name of this Get Routing
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200,Type=typeof(VillaDTO))]

        public ActionResult<VillaDTO> GetVilla(int id)
        //public ActionResult GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Get Villa Error with Id " + id);
                return BadRequest();
            }
            var villa = VillaStore.villalist.FirstOrDefault(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVilla([FromBody]VillaDTO villaDTO) 
        {
            //when commmit the [ApiController] you can use this condetion to check the validation
            //if ( !ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            if (VillaStore.villalist.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest();
            }
            if(villaDTO.Id != 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            villaDTO.Id = VillaStore.villalist.OrderByDescending(u=>u.Id).FirstOrDefault().Id+1;
            VillaStore.villalist.Add(villaDTO);
            //CreateAtRoute take nameRoute and value for run or get the link try this code to know
            return CreatedAtRoute("GetVilla", new {id=villaDTO.Id},villaDTO);
        }

        [HttpDelete("{id:int}",Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        //when use IActionResult not need to <VillaDTO> not need return value
        public IActionResult DeleteVilla(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var villa=VillaStore.villalist.FirstOrDefault(v => v.Id==id);
            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.villalist.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}",Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id, [FromBody]VillaDTO villaDTO)
        {
            if(villaDTO==null||id!=villaDTO.Id)
            {
                return BadRequest();
            }
            //use this when you have the data in the code not in DataBase
            var villa =VillaStore.villalist.FirstOrDefault(u=>u.Id==id);
            villa.Name= villaDTO.Name;
            villa.sqft= villaDTO.sqft;
            villa.Occupancy= villaDTO.Occupancy;
            return NoContent();

        }


        //to update just name or any field use patch updae
        //need download jsonpatch & newtonsoftjson from nuget packages to use patch update also add this packages in program.cs
        [HttpPatch("{id:int}",Name ="UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id,JsonPatchDocument<VillaDTO> patchDTO)
        {
            if(id==0 || patchDTO == null)
            {
                return BadRequest();
            }
            var villa =VillaStore.villalist.FirstOrDefault(u=>u.Id==id);
            if(villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villa,ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent() ;

        }

    }
}
