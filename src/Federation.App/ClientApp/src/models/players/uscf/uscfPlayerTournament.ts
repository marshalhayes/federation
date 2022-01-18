export class UscfPlayerTournament {
  eventId: string;
  eventName: string;
  eventState: string;
  sectionName: string;
  endDate: Date;

  private constructor(
    eventId: string,
    eventName: string,
    eventState: string,
    sectionName: string,
    endDate: Date,
  ) {
    this.eventId = eventId;
    this.eventName = eventName;
    this.eventState = eventState;
    this.sectionName = sectionName;
    this.endDate = new Date(endDate);
  }

  static fromObject<T extends UscfPlayerTournament = UscfPlayerTournament>(
    obj: T,
  ) {
    return new UscfPlayerTournament(
      obj.eventId,
      obj.eventName,
      obj.eventState,
      obj.sectionName,
      obj.endDate,
    );
  }
}
