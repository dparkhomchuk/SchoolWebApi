import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StudentClassComponent } from './studentclass/studentclass.component';
import { SchoolTableComponent } from './schooltable/schooltable.component';

const routes: Routes = [

   {path:"studentclass/:id", component:StudentClassComponent},
   {path:"", component:SchoolTableComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
