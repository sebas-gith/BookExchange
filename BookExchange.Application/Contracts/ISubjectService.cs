﻿using BookExchange.Application.DTOs.Subjects; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookExchange.Application.Contracts
{
    public interface ISubjectService
    {
        Task<SubjectDto> CreateSubjectAsync(SubjectCreateDto createDto); 
        Task<SubjectDto> GetSubjectByIdAsync(int subjectId);
        Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync();
        Task UpdateSubjectAsync(SubjectUpdateDto updateDto);
        Task DeleteSubjectAsync(int subjectId);
        Task<SubjectDto> GetSubjectByNameAsync(string name);
    }
}
