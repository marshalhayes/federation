import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';
import { Federation } from '../../models/federation';
import { UscfPlayer } from '../../models/players/uscf/uscfPlayer';
import { UscfPlayerTournament } from '../../models/players/uscf/uscfPlayerTournament';

@Injectable({
  providedIn: 'root',
})
export class PlayerService {
  constructor(private readonly httpClient: HttpClient) {}

  getById(id: string, federation: Federation | keyof typeof Federation) {
    return this.httpClient
      .get(`/api/federations/${federation}/${encodeURIComponent(id)}`)
      .pipe(
        map(res => UscfPlayer.fromObject(res as any)),
        catchError(() => of(null)),
      );
  }

  getTournamentsForPlayer(
    id: string,
    federation: Federation | keyof typeof Federation,
    offset: number,
    limit: number,
  ) {
    return this.httpClient
      .get<{ total: number; results: Array<UscfPlayerTournament> }>(
        `/api/federations/${federation}/${encodeURIComponent(id)}/tournaments`,
        {
          params: {
            offset,
            limit,
          },
        },
      )
      .pipe(
        map(res => ({
          total: res.total,
          results: res.results.map(t => UscfPlayerTournament.fromObject(t)),
        })),
        catchError(() => of(null)),
      );
  }
}
