import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StudentComponent } from './student/student.component';
import { StudentClassComponent } from './studentclass/studentclass.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from 'src/material-module';
import { SchoolTableComponent } from './schooltable/schooltable.component';
import { HeaderLayoutComponent } from './layout/header-layout/header-layout.component';
import { FooterLayoutComponent } from './layout/footer-layout/footer-layout.component';
import { CreateStudentDialogComponent } from './modals/create-student-dialog/create-student-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SchoolService } from './services/school.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    StudentComponent,
    StudentClassComponent,
    SchoolTableComponent,
    HeaderLayoutComponent,
    FooterLayoutComponent,
    CreateStudentDialogComponent,
      ],
      entryComponents: [
        CreateStudentDialogComponent
      ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule ,
    MaterialModule,
    HttpClientModule
  ],
  providers: [SchoolService],
  bootstrap: [AppComponent]
})
export class AppModule { }
