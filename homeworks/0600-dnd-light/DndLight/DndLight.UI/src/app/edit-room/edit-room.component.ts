import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { DndApiClientService } from '../dnd-api-client.service';
import { RoomDetailsResult } from '../model/roomDetailsResult';
import { RoomSummary } from '../model/roomSummary';

@Component({
  selector: 'app-edit-room',
  templateUrl: './edit-room.component.html',
  styleUrls: ['./edit-room.component.css']
})
export class EditRoomComponent implements OnInit {
  id!: number;
  room?: RoomDetailsResult;
  rooms?: Observable<RoomSummary[]>;

  constructor(private apiClient: DndApiClientService, private route: ActivatedRoute) {
    route.paramMap.subscribe(p => this.id = parseInt(p.get('id') ?? ''));
  }

  ngOnInit(): void {
    this.rooms = this.apiClient.getRooms();
    this.apiClient.getRoomDetails(this.id).subscribe(data => this.room = data);
  }
}
