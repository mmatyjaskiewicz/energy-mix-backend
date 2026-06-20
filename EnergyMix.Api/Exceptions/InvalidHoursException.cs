namespace EnergyMix.Api.Exceptions;

public class InvalidHoursException : Exception
{
    public InvalidHoursException() : base("Hours must be between 1 and 6.") { }
}