using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using School.Interface;
using School.Models;
using School.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace School.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class GroupController : ControllerBase
    {
        IGroupService _groupService;
        IMapper _mapper;
        public GroupController(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }

        [HttpPost("group")]
        public async Task<ActionResult> Add(GroupModel groupModel)
        {
            GroupDTO groupDTO = _mapper.Map<GroupDTO>(groupModel);
            await _groupService.Create(groupDTO);
            return Ok();
        }

        //[HttpDelete("group/{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    await _groupService.Delete(id);
        //    return Ok();
        //}

        //[HttpPut("group")]
        //public async Task<ActionResult> Update(GroupModel grouptModel)
        //{
        //    GroupDTO groupDTO = _mapper.Map<GroupDTO>(grouptModel);
        //    await _groupService.Update(groupDTO);
        //    return Ok();
        //}

        [HttpGet("group/{id}")]
        public async Task<ActionResult> Get(int id)
        {
            GroupDTO groupDTO = await _groupService.Get(id);
            return Ok(_mapper.Map<GroupModel>(groupDTO));
        }

        [HttpGet("groupWithPlaces/{id}")]
        public async Task<ActionResult> GetGroupWithFreePlaces(int id)
        {
            GroupDTOWithFreePlaces groupDtoWithFreePlaces = await _groupService.GetWithFreePlace(id);
            return Ok(_mapper.Map<GroupModelWithFreePlaces>(groupDtoWithFreePlaces));
        }

        [HttpGet("groups")]
        public async Task<ActionResult> GetAll()
        {
            IEnumerable<GroupDTO> groups = await _groupService.GetAll();
            return Ok(_mapper.Map<IEnumerable<GroupModel>>(groups));
        }

        [HttpPut("group/{groupId}/student/{studentId}")]
        public async Task<ActionResult> AddStudentToGroup(int groupId, int studentId)
        {
            await _groupService.AssignStudentToGroup(studentId, groupId);
            return Ok();
        }

        [HttpDelete("group/{groupId}/student/{studentId}")]
        public async Task<ActionResult> DeleteStudentFromGroup(int groupId, int studentId)
        {
            await _groupService.KickStudentFromGroup(studentId, groupId);
            return Ok();
        }

        [HttpGet("studentsByGroup/{id}")]
        public async Task<ActionResult> GetStudentsOfGroup(int id)
        {
            IEnumerable<StudentDTO> studentDTOs = await _groupService.GetStudentsByGroupId(id);
            return Ok(_mapper.Map<IEnumerable<StudentModel>>(studentDTOs));
        }


    }
}
