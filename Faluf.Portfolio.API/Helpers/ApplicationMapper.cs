using AutoMapper;
using Faluf.Portfolio.Core.Domain;
using Faluf.Portfolio.Core.DTOs.Request;
using Faluf.Portfolio.Core.DTOs.Response;

namespace Faluf.Portfolio.API.Helpers;

public class ApplicationMapper : Profile
{
	public ApplicationMapper()
	{
		CreateMap<ApplicationUser, UserDTO>();
		CreateMap<Subject, SubjectDTO>();
		CreateMap<Document, DocumentDTO>();
		CreateMap<Log, Log>();

		CreateMap<UserModel, ApplicationUser>();
		CreateMap<SubjectModel, Subject>();
		CreateMap<DocumentModel, Document>();
	}
}