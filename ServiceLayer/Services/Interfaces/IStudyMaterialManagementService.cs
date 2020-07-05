﻿using ServiceLayer.DTO;
using ServiceLayer.DTO.Materials;
using ServiceLayer.ErrorHandling;
using ServiceLayer.Students.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface IStudyMaterialManagementService
    {
        Task<HashSet<DetailedSemesterDTO>> GetSemestersWithAllInformation();
        Task<List<CourseCategoryDTO>> GetCoursesByYear(int year);
        Task<ResultMessage<bool>> SaveStudyMaterials(int categoryId, int studentId, List<BasicFileInfo> basicFileInfo);
        Task<ResultMessage<BasicFileInfo>> DownloadStudyMaterial(int studyMaterialId);
        Task<int> Count(int courseId, string searchFor = null, int[] categories = null);
        Task<List<DetailedStudyMaterialDTO>> GetDetailedStudyMaterials<TKey>(int courseId, int offset, int itemsCount, string searchFor = null, int[] categories = null,
                                                                             Func<DetailedStudyMaterialDTO, TKey> sortKeySelector = null, bool asc = true);

        Task<ResultMessage<bool>> Rate(int studyMaterialId, int studentId, int authorId, int rating);
        Task<List<DetailedStudyMaterialDTO>> AppendRating(int studentId, List<DetailedStudyMaterialDTO> studyMaterials);
    }
}
