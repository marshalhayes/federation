import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';
import { Federation } from '../../models/federation';
import { UscfPlayer } from '../../models/players/uscf/uscfPlayer';

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
}
