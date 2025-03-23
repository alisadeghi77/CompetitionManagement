using Domain.Entities;
using Domain.Enums;

namespace Application.Participants.GetParticipantList;

public record ParticipantDto(
    long Id,
    string ParticipantId,
    string ParticipantFirstName,
    string ParticipantLastName,
    string? ParticipantPhoneNumber,
    string CoachId,
    string CoachFirstName,
    string CoachLastName,
    string? CoachPhoneNumber,
    RegisterStatus Status,
    List<ParticipantParam>? RegisterParams);