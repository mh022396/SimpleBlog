import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { Category } from '../models/category.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';

@Component({
  selector: 'app-edit-category',
  templateUrl: './edit-category.component.html',
  styleUrls: ['./edit-category.component.css']
})
export class EditCategoryComponent implements OnInit, OnDestroy {

  id: string | null = null;

  paramsSub?: Subscription;

  editSub?: Subscription;

  category?: Category;

  constructor(private route: ActivatedRoute, private categoryService: CategoryService, private router: Router) {
  }

  ngOnInit(): void {
    this.paramsSub = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');
        if (this.id != null) {
          this.editSub = this.categoryService.getCategoryById(this.id).subscribe({ next: (response) => { this.category = response; } });
        }
      }
    });
  }

  onFormSubmit(): void {
    const updateCategoryReuest: UpdateCategoryRequest = {
      name: this.category?.name ?? '',
      urlHandle: this.category?.urlHandle ?? ''
    };

    if (this.id != null) {
      this.categoryService.updateCategory(this.id, updateCategoryReuest).subscribe({ next: (response) => { this.router.navigateByUrl('/admin/categories') } });
    }
  }

  onDelete(): void {
    if (this.id != null) {
      this.categoryService.deleteCategory(this.id).subscribe({ next: () => { this.router.navigateByUrl('/admin/categories') } });
    }
  }

  ngOnDestroy(): void {
    this.paramsSub?.unsubscribe();
    this.editSub?.unsubscribe();
  }
}
