export class UscfPlayer {
  playerId: string;
  playerName: string | null;

  regularRating: string | null;
  quickRating: string | null;
  blitzRating: string | null;

  onlineRegularRating: string | null;
  onlineQuickRating: string | null;
  onlineBlitzRating: string | null;

  state: string | null;

  gender: string | null;

  expiresOn: Date | null;

  lastChangeDate: Date | null;

  overallRanking: number | null;
  overallRankingOutOf: number | null;

  stateRanking: number | null;
  stateRankingOutOf: number | null;

  get statePercentile() {
    if (this.stateRanking === null || this.stateRankingOutOf === null) {
      return null;
    }

    return calculatePercentile(this.stateRanking, this.stateRankingOutOf);
  }

  get overallPercentile() {
    if (this.overallRanking === null || this.overallRankingOutOf === null) {
      return null;
    }

    return calculatePercentile(this.overallRanking, this.overallRankingOutOf);
  }

  private constructor(
    playerId: string,
    playerName: string | null,
    regularRating: string | null,
    quickRating: string | null,
    blitzRating: string | null,
    onlineRegularRating: string | null,
    onlineQuickRating: string | null,
    onlineBlitzRating: string | null,
    state: string | null,
    gender: string | null,
    expiresOn: Date | null,
    lastChangeDate: Date | null,
    overallRanking: number | null,
    overallRankingOutOf: number | null,
    stateRanking: number | null,
    stateRankingOutOf: number | null,
  ) {
    this.playerId = playerId;
    this.playerName = playerName;

    this.regularRating = regularRating;
    this.quickRating = quickRating;
    this.blitzRating = blitzRating;

    this.onlineRegularRating = onlineRegularRating;
    this.onlineQuickRating = onlineQuickRating;
    this.onlineBlitzRating = onlineBlitzRating;

    this.state = state;

    this.gender = gender;

    this.expiresOn = expiresOn === null ? null : new Date(expiresOn);

    this.lastChangeDate =
      lastChangeDate === null ? null : new Date(lastChangeDate);

    this.overallRanking = overallRanking;
    this.overallRankingOutOf = overallRankingOutOf;

    this.stateRanking = stateRanking;
    this.stateRankingOutOf = stateRankingOutOf;
  }

  static fromObject(obj: any) {
    return new UscfPlayer(
      obj.playerId,
      obj.playerName,
      obj.regularRating,
      obj.quickRating,
      obj.blitzRating,
      obj.onlineRegularRating,
      obj.onlineQuickRating,
      obj.onlineBlitzRating,
      obj.state,
      obj.gender,
      obj.expiresOn,
      obj.lastChangeDate,
      obj.overallRanking,
      obj.overallRankingOutOf,
      obj.stateRanking,
      obj.stateRankingOutOf,
    );
  }
}

const calculatePercentile = (n: number, total: number) =>
  (((total - n) / total) * 100).toFixed(2);
