﻿using System;
using System.IO;

namespace ServiceLayer.Shared
{
    /// <summary>
    /// This class contains constant, predefined top folder names, and based on specific additional 
    /// information, can be used to generate URIs.
    /// </summary>
    public static class PathBuilder
    {
        public const string IdSeparator = "_";
        private const string studyMaterialsRootDirectoryName = "studyMaterials";
        private const string studyMaterialsCourseDirectoryNamePrefix = "course";
        private const string studyMaterialsCategoryNamePrefix = "category";
        private const string profilePictureRootDirectoryName = "profile_pictures";
        private const string fileNameDataFormat = "dd-MM-yyyy-HH-mm-ss";

        public static string BuildPathForStudyMaterial(int courseId, int categoryId, string fileName) =>
            Path.Combine(studyMaterialsRootDirectoryName,
                         BuildName(studyMaterialsCourseDirectoryNamePrefix, courseId),
                         BuildName(studyMaterialsCategoryNamePrefix, categoryId),
                         DateTime.Now.ToString(fileNameDataFormat) + IdSeparator + fileName);

        public static string BuildPathForProfileImage() =>
            Path.Combine(profilePictureRootDirectoryName, Path.GetRandomFileName());

        public static string BuildName(string prefix, long id) => prefix + IdSeparator + id;
    }
}
