using TrackEasy.Domain.Coaches;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.Trains;

namespace TrackEasy.Infrastructure.Queries.Operators;

public static class Extensions
{
    public static IQueryable<Operator> WithOperatorId(this IQueryable<Operator> queryable, Guid? operatorId)
    {
        if (operatorId == null || operatorId == Guid.Empty)
            return queryable;

        return queryable.Where(op => op.Id == operatorId);
    }
    
    public static IQueryable<Operator> WithName(this IQueryable<Operator> queryable, string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return queryable;

        return queryable.Where(op => op.Name.Contains(name));
    }
    
    public static IQueryable<Operator> WithCode(this IQueryable<Operator> queryable, string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return queryable;

        return queryable.Where(op => op.Code.Contains(code));
    }
    
    public static IQueryable<Coach> WithCoachId(this IQueryable<Coach> queryable, Guid? coachId)
    {
        if (coachId == null || coachId == Guid.Empty)
            return queryable;

        return queryable.Where(c => c.Id == coachId);
    }
    
    public static IQueryable<Coach> WithCoachCode(this IQueryable<Coach> queryable, string? coachCode)
    {
        if (string.IsNullOrWhiteSpace(coachCode))
            return queryable;

        return queryable.Where(c => c.Code.Contains(coachCode));
    }

    public static IQueryable<Coach> WithOperatorId(this IQueryable<Coach> queryable, Guid? operatorId)
    {
        if (operatorId == null || operatorId == Guid.Empty)
            return queryable;
        
        return queryable.Where(c => c.OperatorId == operatorId);
    }
    
    public static IQueryable<Train> WithOperatorId(this IQueryable<Train> queryable, Guid? operatorId)
    {
        if (operatorId == null || operatorId == Guid.Empty)
            return queryable;
        
        return queryable.Where(c => c.OperatorId == operatorId);
    }
    
    public static IQueryable<Train> WithTrainName(this IQueryable<Train> queryable, string? trainName)
    {
        if (string.IsNullOrWhiteSpace(trainName))
            return queryable;

        return queryable.Where(t => t.Name.Contains(trainName));
    }
    
    public static IQueryable<Train> WithTrainId(this IQueryable<Train> queryable, Guid? trainId)
    {
        if (trainId == null || trainId == Guid.Empty)
            return queryable;

        return queryable.Where(t => t.Id == trainId);
    }
}