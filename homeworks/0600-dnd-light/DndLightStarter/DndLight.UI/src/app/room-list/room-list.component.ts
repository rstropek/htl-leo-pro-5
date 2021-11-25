import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DndApiClientService } from '../dnd-api-client.service';
import { RoomSummary } from '../model/models';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.css']
})
export class RoomListComponent implements OnInit {

  constructor(private apiClient: DndApiClientService, private router: Router) { }

  ngOnInit(): void {
    throw new Error('Not implemented yet');
  }
}
