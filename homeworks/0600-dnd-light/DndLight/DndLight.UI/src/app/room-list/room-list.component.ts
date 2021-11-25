import { HttpParams } from '@angular/common/http';
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

  rooms?: Observable<RoomSummary[]>;

  constructor(private apiClient: DndApiClientService, private router: Router) { }

  ngOnInit(): void {
    this.refresh();
  }

  refresh() {
    this.rooms = this.apiClient.getRooms();
  }

  deleteRoom(id: number) {
    this.apiClient.deleteRoom(id).subscribe(() => this.refresh());
  }

  editRoom(id: number) {
    this.router.navigate(['edit-room', id]);
  }
}
