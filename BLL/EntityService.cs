using BLL;
using System;

namespace BLL
{
    public abstract class EntityService<TEntity, TDto> where TEntity : DAL.Person
        where TDto : BLL.PersonDto
    {
        protected readonly DAL.EntityContext<TEntity> _context;

        protected EntityService(DAL.IDataProvider<TEntity> dataProvider, string filePath)
        {
            _context = new DAL.EntityContext<TEntity>(dataProvider, filePath);
        }

        protected abstract TEntity MapToEntity(TDto dto);
        protected abstract TDto MapToDto(TEntity entity);

        public void Add(TDto dto)
        {
            if (string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.LastName))
            {
                throw new BusinessLogicException("First name and last name must be specified.");
            }
            try
            {
                TEntity entity = MapToEntity(dto);
                _context.Add(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException("Error adding entity to data layer.", ex);
            }
        }

        public List<TDto> GetAll()
        {
            try
            {
                return _context.GetAll().Select(MapToDto).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException("Error retrieving entities from data layer.", ex);
            }
        }

        public TDto Find(string firstName, string lastName, string passport)
        {
            try
            {
                TEntity entity = _context.GetAll()
                    .FirstOrDefault(e =>
                        e.FirstName == firstName &&
                        e.LastName == lastName &&
                        e.Passport == passport);

                if (entity == null)
                {
                    throw new BusinessLogicException("Person not found.");
                }

                return MapToDto(entity);
            }
            catch (Exception ex) when (!(ex is BusinessLogicException))
            {
                throw new BusinessLogicException("Error searching for person.", ex);
            }
        }

        public void Remove(string firstName, string lastName, string passport)
        {
            try
            {
                List<TEntity> entities = _context.GetAll();
                TEntity entityToRemove = entities.FirstOrDefault(e =>
                    e.FirstName == firstName &&
                    e.LastName == lastName &&
                    e.Passport == passport);

                if (entityToRemove == null)
                {
                    throw new BusinessLogicException("Person not found for removal.");
                }

                entities.Remove(entityToRemove);
                _context.SaveAll(entities);
            }
            catch (Exception ex) when (!(ex is BusinessLogicException))
            {
                throw new BusinessLogicException("Error removing person.", ex);
            }
        }
    }

    public class StudentService : EntityService<DAL.Student, StudentDto>
    {
        public StudentService(DAL.IDataProvider<DAL.Student> dataProvider, string filePath)
            : base(dataProvider, filePath) { }

        protected override DAL.Student MapToEntity(StudentDto dto)
        {
            return new DAL.Student(dto.FirstName, dto.LastName, dto.Passport, dto.StudentID, dto.Course, dto.MilitaryID);
        }

        protected override StudentDto MapToDto(DAL.Student entity)
        {
            return new StudentDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Passport = entity.Passport,
                StudentID = entity.StudentID,
                Course = entity.Course,
                MilitaryID = entity.MilitaryID
            };
        }

        public List<StudentDto> FindFifthYearStudentsWheServed()
        {
            List<StudentDto> allStudents = GetAll();

            return allStudents
                .Where(s => s.Course == 5 && !string.IsNullOrEmpty(s.MilitaryID) && s.MilitaryID != "N/A")
                .ToList();
        }
    }

    public class FootballPlayerService : EntityService<DAL.FootballPlayer, FootballPlayerDto>
    {
        public FootballPlayerService(DAL.IDataProvider<DAL.FootballPlayer> dataProvider, string filePath)
            : base(dataProvider, filePath) { }

        protected override DAL.FootballPlayer MapToEntity(FootballPlayerDto dto)
        {
            return new DAL.FootballPlayer(dto.FirstName, dto.LastName, dto.Passport, dto.Team);
        }

        protected override FootballPlayerDto MapToDto(DAL.FootballPlayer entity)
        {
            return new FootballPlayerDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Passport = entity.Passport,
                Team = entity.Team
            };
        }
    }

    public class LawyerService : EntityService<DAL.Lawyer, LawyerDto>
    {
        public LawyerService(DAL.IDataProvider<DAL.Lawyer> dataProvider, string filePath)
            : base(dataProvider, filePath) { }

        protected override DAL.Lawyer MapToEntity(LawyerDto dto)
        {
            return new DAL.Lawyer(dto.FirstName, dto.LastName, dto.Passport, dto.Company);
        }

        protected override LawyerDto MapToDto(DAL.Lawyer entity)
        {
            return new LawyerDto
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Passport = entity.Passport,
                Company = entity.Company
            };
        }
    }
}