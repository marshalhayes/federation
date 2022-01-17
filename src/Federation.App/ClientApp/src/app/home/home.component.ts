import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Federation } from '../../models/federation';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  readonly playerSearchForm: FormGroup;

  searchIsLoading = false;
  federationNames = Object.keys(Federation);

  get selectedFederation() {
    return this.playerSearchForm.get('playerFederation')!.value;
  }

  constructor(
    titleService: Title,
    formBuilder: FormBuilder,
    private readonly router: Router,
  ) {
    this.playerSearchForm = formBuilder.group({
      playerId: ['', [Validators.required]],
      playerFederation: [Federation.USCF, [Validators.required]],
    });

    titleService.setTitle('Federation.App');
  }

  async onPlayerSearchFormSubmit($event: Event) {
    $event.preventDefault();

    if (this.playerSearchForm.valid) {
      try {
        this.searchIsLoading = true;

        const id = this.playerSearchForm.get('playerId')?.value;
        const federation = this.playerSearchForm.get('playerFederation')?.value;

        const url = `/players/${encodeURIComponent(
          federation,
        )}/${encodeURIComponent(id)}`.toLowerCase();

        await this.router.navigate([url]);
      } finally {
        this.searchIsLoading = false;
      }
    }
  }
}
