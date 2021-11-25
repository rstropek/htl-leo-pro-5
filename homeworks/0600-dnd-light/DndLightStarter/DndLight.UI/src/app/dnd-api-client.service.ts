import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { NewRoom, NewRoomDetails, Room, RoomDetailsResult, RoomSummary } from './model/models';

@Injectable({
  providedIn: 'root'
})
export class DndApiClientService {

  constructor(private httpClient: HttpClient) { }

  public getRooms() : Observable<RoomSummary[]> {
    return this.httpClient.get<RoomSummary[]>(`${environment.apiBaseUrl}/api/rooms`);
  }

  public deleteRoom(id: number) : Observable<unknown> {
    return this.httpClient.delete(`${environment.apiBaseUrl}/api/rooms/${id}`);
  }

  public addRoom(room: NewRoom): Observable<Room> {
    return this.httpClient.post<Room>(`${environment.apiBaseUrl}/api/rooms`, room);
  }

  public getRoomDetails(id: number) : Observable<RoomDetailsResult> {
    return this.httpClient.get<RoomDetailsResult>(`${environment.apiBaseUrl}/api/rooms/${id}`);
  }

  public updateRoom(id: number, room: NewRoomDetails) : Observable<RoomDetailsResult> {
    return this.httpClient.put<RoomDetailsResult>(`${environment.apiBaseUrl}/api/rooms/${id}`, room);
  }
}
