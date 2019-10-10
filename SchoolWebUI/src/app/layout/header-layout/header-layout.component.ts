import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header-layout',
  templateUrl: './header-layout.component.html',
  styleUrls: ['./header-layout.component.scss']
})
export class HeaderLayoutComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }
  getClasses():void {
    console.log("get Classes");
  }
  getTeachers():void {
    console.log("get TeachersClasses");
  }
  getStudents():void{
    console.log("get Students");
  }
}
