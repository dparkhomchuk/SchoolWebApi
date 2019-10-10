import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CreateStudentDialogComponent } from '../modals/create-student-dialog/create-student-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { StudentsService } from '../services/students.service';
export interface StudentsTable {
  Id: number;
  Name: string;
  SurName: string;
}

@Component({
  selector: 'app-studentclass',
  templateUrl: './studentclass.component.html',
  styleUrls: ['./studentclass.component.scss']
})
export class StudentClassComponent implements OnInit {
  students: StudentsTable[];
  displayedColumns: string[] = ['Id', 'Name', 'SurName'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
  classId:number;
  constructor(public dialog: MatDialog,
    private activatedRoute: ActivatedRoute,private service:StudentsService) {
    this.classId = this.activatedRoute.snapshot.params.id;
     }

  ngOnInit() {
    this.getStudentsTable();
  }

getStudentsTable():Array<StudentsTable>{
  console.log(this.classId);
  this.service.getStudentsByClassId(this.classId).subscribe(s=>{
    console.log(s)
  })
  this.students=[
  {Id:1, Name:"Andrew", SurName:"Fine"},
  {Id:1, Name:"Robert", SurName:"Largent"},
  {Id:1, Name:"Bernard", SurName:"Reyes"},
  {Id:1, Name:"Ihor", SurName:"Stankevych"},
  {Id:1, Name:"Oleksandr", SurName:"Kozlenko"},
  ];
  return this.students;
}
addStudent():void{
  const dialogRef = this.dialog.open(CreateStudentDialogComponent, {
    width: '300px',
   // data: {name: this.name, animal: this.animal}
  });

  dialogRef.afterClosed().subscribe(result => {
    console.log('The dialog was closed', result);
    //this.animal = result;
  });
}
}
