using AutoMapper;
using BL.IServices;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BL.Services;

public class GroupService : IGroupService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _groupMapper;
	private readonly ILogService _logService;


	public GroupService(IUnitOfWork unitOfWork, IConfiguration configuration, IMapper GroupMapper, ILogService logService)
	{
		_unitOfWork = unitOfWork;
		_groupMapper = GroupMapper;
		_logService = logService;
	
	}
	public ICollection<GroupDto> GetAll()
	{
		var allGroups = _unitOfWork.Group.GetAll(includeProperties: "Tag");
		_logService.Log("info", "All Groups were Fetched");
		return _groupMapper.Map<ICollection<GroupDto>>(allGroups).ToList();
	}


	public GroupDto? GetById(int id)
	{
		var requestedGroup = _unitOfWork.Group.GetFirstOrDefault(p => p.Id == id);
		_logService.Log("info", "Group with id " + id + " was Fetched");
		return requestedGroup == null ? null : _groupMapper.Map<GroupDto>(requestedGroup);

	}

	public GroupDto? Create(GroupDto group)
	{
		var existingGroup = _unitOfWork.Group.GetFirstOrDefault(u => u.Name == group.Name);
		if (existingGroup != null)
		{
			_logService.Log("error", "Group with name " + group.Name + " already exists and could not be created");
			throw new InvalidOperationException("Group already exists");
		}

		group.TagName = "Test";
		var newGroup = _groupMapper.Map<Group>(group);
		
		_unitOfWork.Group.Add(newGroup);
		_unitOfWork.Save();
		_logService.Log("info", "Group with name " + group.Name + " was Created");
		return _groupMapper.Map<GroupDto>(newGroup);

	}
	public GroupDto? Update(int id, GroupDto group)
	{
		var GroupForUpdate = _unitOfWork.Group.GetFirstOrDefault(p => p.Id == id, "Tag.TagName");
		if (GroupForUpdate == null) return null;


		GroupForUpdate.Name = group.Name;
		

		_unitOfWork.Save();

		_logService.Log("info", "Group with name " + group.Name + " was Updated");
		return _groupMapper.Map<GroupDto>(GroupForUpdate);
	}


	public GroupDto? Delete(int id)
	{
		var toDelete = _unitOfWork.Group.GetFirstOrDefault(p => p.Id == id);
		if (toDelete == null) return null;

		

		_unitOfWork.Group.Delete(toDelete);
		_unitOfWork.Save();
		_logService.Log("info", "Group with name " + toDelete.Name + " was Deleted");
		return _groupMapper.Map<GroupDto>(toDelete);
	}



	public int GetCount() => _unitOfWork.Group.GetAll().Count();


	
}