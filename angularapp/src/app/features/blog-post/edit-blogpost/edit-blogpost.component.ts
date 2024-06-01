import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPostService } from '../services/blog-post.service';
import { BlogPost } from '../models/blog-post.model';
import { CategoryService } from '../../category/services/category.service';
import { Category } from '../../category/models/category.model';
import { UpdateBlogPost } from '../models/update-blog-post.model';

@Component({
  selector: 'app-edit-blogpost',
  templateUrl: './edit-blogpost.component.html',
  styleUrls: ['./edit-blogpost.component.css']
})
export class EditBlogpostComponent implements OnInit, OnDestroy {

  id: string | null = null;

  model?: BlogPost;

  routeSub?: Subscription;

  categories$?: Observable<Category[]>;

  selectedCategories?: string[];

  updateBlogPostSub?: Subscription;
  getBlogPostSub?: Subscription;

  constructor(private route: ActivatedRoute, private blogPostService: BlogPostService, private categoryService: CategoryService, private router: Router) {
  }
  
  ngOnInit(): void {
    this.categoryService.getAllCategories();
    this.routeSub = this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id')
        if (this.id !== null) {
          this.blogPostService.getBlogPostById(this.id).subscribe({
            next: (response) => {
              this.model = response;
              this.selectedCategories = response.categories.map(c => c.id);
            }
          });
        }
      }
    })
  }

  onFormSubmit(): void {
    if (this.model != null && this.id != null) {
      var updateBlogPost: UpdateBlogPost = {
        author: this.model.author,
        content: this.model.content,
        categories: this.selectedCategories ?? [],
        featuredImageUrl: this.model.featuredImageUrl,
        isVisble: this.model.isVisble,
        publishedDate: this.model.publishedDate,
        shortDescription: this.model.shortDescription,
        title: this.model.title,
        urlHandle: this.model.urlHandle
      }
      this.updateBlogPostSub = this.blogPostService.updateBlogPost(this.id, updateBlogPost).subscribe({ next: (reponse) => { this.router.navigateByUrl('/admin/blogposts'); } })
    }
  }

  onDelete(): void {
    if (this.id !== null) {
      this.blogPostService.deleteBlogPostById(this.id).subscribe({ next: (reponse) => { this.router.navigateByUrl('/admin/blogposts') } })
    }
  }

  ngOnDestroy(): void {
    this.routeSub?.unsubscribe();
    this.updateBlogPostSub?.unsubscribe();
    this.getBlogPostSub?.unsubscribe();
  }

}
