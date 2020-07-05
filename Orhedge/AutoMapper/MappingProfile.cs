﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Orhedge.Helpers;
using Orhedge.ViewModels;
using Orhedge.ViewModels.Forum;
using Orhedge.ViewModels.Student;
using Orhedge.ViewModels.StudyMaterial;
using Orhedge.ViewModels.TechnicalSupport;
using ServiceLayer.DTO;
using ServiceLayer.DTO.Forum;
using ServiceLayer.DTO.Materials;
using ServiceLayer.DTO.Student;
using ServiceLayer.Models;
using ServiceLayer.Students.Shared;
using System;
using System.Collections.Generic;

namespace Orhedge.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapStudyMaterials();
            MapStudents();
            MapTechnicalSupport();
            MapForum();
        }

        private void MapTechnicalSupport()
        {
            CreateMap<ChatMessageDTO, ChatMessageViewModel>();
        }

        public void MapStudyMaterials()
        {
            CreateMap<CourseDTO, IndexCourseViewModel>();
            CreateMap<DetailedSemesterDTO, SemesterViewModel>().ForMember(dest => dest.Courses, conf => conf.MapFrom(src => src.Courses)).ReverseMap();
            CreateMap<CategoryDTO, CategoryViewModel>();
            CreateMap<CourseCategoryDTO, CourseCategoryViewModel>()
                .ForMember(dest => dest.CourseId, opts => opts.MapFrom(courseCat => courseCat.Course.CourseId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(courseCat => courseCat.Course.Name))
                .ForMember(dest => dest.Categories, opts => opts.MapFrom(courseCat => courseCat.Categories));
            CreateMap<DetailedStudyMaterialDTO, StudyMaterialViewModel>().ForMember(dest => dest.GivenRating, conf => { conf.MapFrom(src => src.GivenRating); conf.NullSubstitute(0); });
            CreateMap<EditStudyMaterialViewModel, StudyMaterialDTO>();
            CreateMap<IFormFile, BasicFileInfo>().ConvertUsing<FormFileToBasicFileInfoConverter>();
        }

        private void MapStudents()
        {
            CreateMap<RegisterFormViewModel, RegisterFormDTO>()
               .ForMember(dest => dest.Index, opts => opts.MapFrom(src => src.IndexNumber));
            CreateMap<RegisterViewModel, RegisterUserDTO>();
            CreateMap<LoginViewModel, LoginRequest>();
            CreateMap<StudentDTO, EditProfileViewModel>()
                .ForMember(dest => dest.Photo, opts => opts.Ignore())
                .ForMember(dest => dest.PhotoVersion, opts => opts.Condition(s => s.Photo != null));
            CreateMap<EditProfileViewModel, ProfileUpdateDTO>()
                .ForMember(dest => dest.Photo, opts =>
                {
                    opts.MapFrom(src => new FormFile(src.Photo));
                    opts.Condition(src => src.Photo != null);
                });

            CreateMap<ChangePasswordViewModel, UpdatePasswordDTO>();
            CreateMap<StudentDTO, ViewModels.Admin.StudentViewModel>().ForMember(dest => dest.PhotoVersion, opts => opts.Condition(src => src.Photo != null)).ReverseMap();
            CreateMap<ViewModels.Admin.EditStudentViewModel, StudentDTO>().ReverseMap();
        }

        public void MapForum()
        {
            CreateMap<TopicListDTO, TopicSelectionViewModel[]>()
                .ConvertUsing((topicList, ctx) =>
                {
                    List<TopicSelectionViewModel> result = new List<TopicSelectionViewModel>();

                    TopicDTO[] topics = topicList.Topics;
                    StudentDTO[] authors = topicList.Authors;
                    int[] postCounts = topicList.PostCounts;

                    for (int i = 0; i < topics.Length; ++i)
                    {

                        TopicSelectionViewModel topicVm = new TopicSelectionViewModel
                        {
                            Author = new AuthorViewModel
                            {
                                Id = authors[i].StudentId,
                                Username = authors[i].Username
                            },
                            Title = topics[i].Title,
                            PostCount = postCounts[i],
                            LastPost = topics[i].LastPost
                        };

                        result.Add(topicVm);
                    }

                    return result.ToArray();
                });
            CreateMap<StudentDTO, AuthorViewModel>()
                .ConvertUsing((student, author) =>
                {
                    AuthorViewModel result = new AuthorViewModel
                    {
                        Id = student.StudentId,
                        Username = student.Username
                    };

                    return result;
                });
            CreateMap<DiscussionPostsDTO, DiscussionPostViewModel[]>()
                .ConvertUsing((discussionPosts, dpa) =>
                {
                    List<DiscussionPostViewModel> result = new List<DiscussionPostViewModel>();
                    DiscussionPostDTO[] posts = discussionPosts.DiscussionPosts;
                    StudentDTO[] authors = discussionPosts.Authors;

                    for (int i = 0; i < posts.Length; ++i)
                    {
                        DiscussionPostViewModel dpvm = new DiscussionPostViewModel
                        {
                            Content = posts[i].Content,
                            Created = posts[i].Created,
                            Edited = posts[i].Edited,
                            Author = new AuthorViewModel
                            {
                                Id = authors[i].StudentId,
                                Username = authors[i].Username
                            }
                        };
                        result.Add(dpvm);
                    }

                    return result.ToArray();
                });
        }
    }
}
