import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DialogData } from '../dialog-data';
import { Validators, FormControl } from '@angular/forms';
import { StudentsService } from 'src/app/services/students.service';
// export class Student{
//   id:number,
//   name:string,
//   surname:string,
//   age:number
// }
@Component({
  selector: 'app-create-student-dialog',
  templateUrl: './create-student-dialog.component.html',
  styleUrls: ['./create-student-dialog.component.scss']
})
export class CreateStudentDialogComponent implements OnInit  {
  age = new FormControl('', [Validators.required, Validators.nullValidator]);
  name = new FormControl('', [Validators.required, Validators.nullValidator]);
  surname = new FormControl('', [Validators.required, Validators.nullValidator]);
  // student:Student;
  constructor( public dialogRef: MatDialogRef<CreateStudentDialogComponent>,
     @Inject(MAT_DIALOG_DATA) public data: DialogData, private service:StudentsService) { }
     ngOnInit(){}

     getErrorMessage(formControl:FormControl) {
       if(formControl.hasError('required') ){
         return 'You must enter a value'
       }
    }
    saveStudent(name:string, surname:string,
      age:number):void{
        let student ={name:name, surname:surname, age:age};
       this.service.addStudent(student).subscribe(s=>{console.log(s)});
    }
}
