using AutoMapper;
using BookExchange.Application.DTOs.Subjects;
using BookExchange.Application.Exceptions;
using BookExchange.Domain.Entities;
using BookExchange.Domain.Interfaces;
using BookExchange.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookExchange.Application.Contracts;

namespace BookExchange.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMapper _mapper;

        public SubjectService(ISubjectRepository subjectRepository, IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _mapper = mapper;
        }

        public async Task<SubjectDto> CreateSubjectAsync(SubjectCreateDto createDto)
        {
            // Validar que el nombre de la materia no exista
            var existingSubject = await _subjectRepository.GetSubjectByNameAsync(createDto.Name);
            if (existingSubject != null)
            {
                throw new Exceptions.ApplicationException($"La materia '{createDto.Name}' ya existe.");
            }

            var subject = _mapper.Map<Subject>(createDto);
            await _subjectRepository.AddAsync(subject);
            await _subjectRepository.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<SubjectDto> GetSubjectByIdAsync(int subjectId)
        {
            var subject = await _subjectRepository.GetByIdAsync(subjectId);
            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync()
        {
            var subjects = await _subjectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }

        public async Task UpdateSubjectAsync(SubjectUpdateDto updateDto)
        {
            var subjectToUpdate = await _subjectRepository.GetByIdAsync(updateDto.Id);
            if (subjectToUpdate == null)
            {
                throw new KeyNotFoundException($"Materia con ID {updateDto.Id} no encontrada.");
            }

            // Si el nombre de la materia cambia, verifica que el nuevo nombre no exista ya
            if (updateDto.Name != subjectToUpdate.Name)
            {
                var existingSubjectWithNewName = await _subjectRepository.GetSubjectByNameAsync(updateDto.Name);
                if (existingSubjectWithNewName != null && existingSubjectWithNewName.Id != updateDto.Id)
                {
                    throw new Exceptions.ApplicationException($"Ya existe otra materia con el nombre '{updateDto.Name}'.");
                }
            }

            _mapper.Map(updateDto, subjectToUpdate);
            _subjectRepository.Update(subjectToUpdate);
            await _subjectRepository.SaveChangesAsync();
        }

        public async Task DeleteSubjectAsync(int subjectId)
        {
            var subjectToDelete = await _subjectRepository.GetByIdAsync(subjectId);
            if (subjectToDelete == null)
            {
                throw new KeyNotFoundException($"Materia con ID {subjectId} no encontrada.");
            }

         
            _subjectRepository.Remove(subjectToDelete);
            await _subjectRepository.SaveChangesAsync();
        }

        public async Task<SubjectDto> GetSubjectByNameAsync(string name)
        {
            var subject = await _subjectRepository.GetSubjectByNameAsync(name);
            return _mapper.Map<SubjectDto>(subject);
        }
    }
}