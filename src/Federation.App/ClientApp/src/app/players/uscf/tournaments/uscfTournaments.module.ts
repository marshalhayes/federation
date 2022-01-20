import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UscfTournamentsComponent } from './uscfTournaments.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [CommonModule, FormsModule],
  declarations: [UscfTournamentsComponent],
  exports: [UscfTournamentsComponent],
})
export class UscfTournamentsModule {}
