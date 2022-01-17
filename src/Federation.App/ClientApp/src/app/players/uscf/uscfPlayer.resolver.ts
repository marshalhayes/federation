import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from '@angular/router';
import { PlayerService } from '../player.service';
import { Federation } from '../../../models/federation';
import { of } from 'rxjs';
import { Injectable } from '@angular/core';
import { UscfPlayer } from '../../../models/players/uscf/uscfPlayer';

@Injectable({
  providedIn: 'root',
})
export class UscfPlayerResolver implements Resolve<UscfPlayer | null> {
  constructor(private readonly playerService: PlayerService) {}

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const id = route.paramMap.get('uscfId');

    if (id !== null) {
      return this.playerService.getById(id, Federation.USCF);
    }

    return of(null);
  }
}
