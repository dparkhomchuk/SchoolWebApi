import { Component, OnInit } from '@angular/core';
import { SchoolService } from '../services/school.service';
import {Router} from '@angular/router';
export interface ClassTable {
  class: string;
  Id: number;
  count: number;
  free: number;
}
@Component({
  selector: 'app-schooltable',
  templateUrl: './schooltable.component.html',
  styleUrls: ['./schooltable.component.scss']
})
export class SchoolTableComponent implements OnInit {

 displayedColumns: string[] = ['class', 'count', 'free','actions'];
  columnsToDisplay: string[] = this.displayedColumns.slice();
 data: ClassTable[];
  constructor(private datasource: SchoolService,
  private router: Router) { }

  ngOnInit() {
    this.data = this.getSchoolTable();

  }

  getSchoolTable(): Array<ClassTable>
  {
this.datasource.GetClasses().subscribe(d=>{
  console.log(d);
})
    this.data =
     [
      {Id: 1, class: '11-A', count: 35, free: 0},
      {Id: 2, class: '11-B', count: 40, free: 3},
      {Id: 3, class: '10-A', count: 25, free: 4},
      {Id: 4, class: '10-B', count: 26, free: 5},
      {Id: 5, class: '9-A', count: 29, free: 2},
      {Id: 6, class: '9-B', count: 34, free: 7},
      {Id: 7, class: '8-A', count: 18, free: 10},
      {Id: 8, class: '8-B', count: 26, free: 11},
      {Id: 9, class: '7-A', count: 32, free: 8},
      {Id: 10, class: '7-B', count: 37, free: 3},
  ];

      return this.data;
  }

  goToClass(row:ClassTable){
       this.router.navigate(['studentclass/', row.Id])
  }

}
