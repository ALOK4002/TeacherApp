import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, CommonModule, FormsModule, ReactiveFormsModule],
  template: `
    <router-outlet></router-outlet>
  `,
  styles: []
})
export class App {
  title = 'AuthApp';
}
