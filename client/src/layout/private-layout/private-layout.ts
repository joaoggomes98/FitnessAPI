import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from '../header/header';
import { Sidebar } from '../sidebar/sidebar';

@Component({
  selector: 'app-private-layout',
  imports: [RouterOutlet, Header, Sidebar],
  templateUrl: './private-layout.html',
  styleUrl: './private-layout.css'
})
export class PrivateLayout {}