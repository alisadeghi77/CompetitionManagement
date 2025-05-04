using Domain.Entities;

namespace Api.Models;

public record RegisterParticipantRequest(
    string? CoachId,
    string? CoachPhoneNumber,
    long CompetitionId,
    List<ParticipantParam> Params);