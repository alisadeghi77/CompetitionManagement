using Domain.Entities;
using Domain.Enums;

namespace Application.Participants.GetParticipantList;

public record ParticipantDto(
    long Id,
    string ParticipantUserId,
    string ParticipantFullName,
    string? ParticipantPhoneNumber,
    string CoachId,
    string CoachFullName,
    string? CoachPhoneNumber,
    RegisterStatus Status,
    List<ParticipantParam>? RegisterParams);