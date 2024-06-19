<script lang="ts">
	import ChevronLeft from 'lucide-svelte/icons/chevron-left';
	import Home from 'lucide-svelte/icons/home';
	import LineChart from 'lucide-svelte/icons/line-chart';
	import Package from 'lucide-svelte/icons/package';
	import Package2 from 'lucide-svelte/icons/package-2';
	import PanelLeft from 'lucide-svelte/icons/panel-left';
	import Search from 'lucide-svelte/icons/search';
	import Settings from 'lucide-svelte/icons/settings';
	import ShoppingCart from 'lucide-svelte/icons/shopping-cart';
	import UsersRound from 'lucide-svelte/icons/users-round';

	import * as Breadcrumb from '$lib/components/ui/breadcrumb/index.js';
	import { Button } from '$lib/components/ui/button/index.js';
	import * as Card from '$lib/components/ui/card/index.js';
	import * as DropdownMenu from '$lib/components/ui/dropdown-menu/index.js';
	import { Input } from '$lib/components/ui/input/index.js';
	import { Label } from '$lib/components/ui/label/index.js';
	import * as Select from '$lib/components/ui/select/index.js';
	import * as Sheet from '$lib/components/ui/sheet/index.js';
	import * as Tooltip from '$lib/components/ui/tooltip/index.js';
	import { toast } from 'svelte-sonner';
	import { enhance } from '$app/forms';
	import { Toaster } from '$lib/components/ui/sonner';

    export let form;

    if (form?.id) {
        console.log('Editing tenant', form);
        toast.success('Tenant created successfully', { duration: 5000 });
    }
    
</script>

<div class="flex min-h-screen w-full flex-col bg-muted/40">
		<main class="grid flex-1 items-start gap-4 p-4 sm:px-6 sm:py-0 md:gap-4">
			<form method="POST" use:enhance>
				<div class="mx-auto grid max-w-[150rem] auto-rows-max gap-4 md:mx-48">
					<div class="flex items-center gap-4">
						<a href="/tenants">
							<Button variant="outline" size="icon" class="h-7 w-7">
								<ChevronLeft class="h-4 w-4" />
								<span class="sr-only">Back</span>
							</Button>
						</a>
						<h1
							class="flex-1 shrink-0 whitespace-nowrap text-xl font-semibold tracking-tight sm:grow-0"
						>
							New Tenant
						</h1>
						<div class="hidden items-center gap-2 md:ml-auto md:flex">
							<Button variant="outline" size="sm">Discard</Button>
							<Button size="sm" type="submit">Save Tenant</Button>
						</div>
					</div>
					<div class="grid gap-4 md:grid-cols-[1fr_250px] lg:grid-cols-3 lg:gap-8">
						<div class="grid auto-rows-max items-start gap-4 lg:col-span-2 lg:gap-8">
							<Card.Root>
								<Card.Header>
									<Card.Title>Tenant Details</Card.Title>
									<Card.Description>
										This information will be displayed on the tenant profile
									</Card.Description>
								</Card.Header>
								<Card.Content>
									<div class="grid gap-6">
										<div class="grid gap-3">
											<Label for="name">Name</Label>
											<Input id="name" name="name" type="text" class="w-full" />
										</div>
									</div>
								</Card.Content>
							</Card.Root>
						</div>
						<div class="grid auto-rows-max items-start gap-4 lg:gap-8">
							<Card.Root>
								<Card.Header>
									<Card.Title>Tenant Status</Card.Title>
								</Card.Header>
								<Card.Content>
									<div class="grid gap-6">
										<div class="grid gap-3">
											<Label for="status">Status</Label>
											<Select.Root>
												<Select.Trigger id="status" aria-label="Select status">
													<Select.Value placeholder="Select status" />
												</Select.Trigger>
												<Select.Content>
													<Select.Item value="published" label="Active">Active</Select.Item>
													<Select.Item value="archived" label="Archived">Inactive</Select.Item>
												</Select.Content>
											</Select.Root>
										</div>
									</div>
								</Card.Content>
							</Card.Root>
						</div>
					</div>
					<div class="flex items-center justify-center gap-2 md:hidden">
						<Button variant="outline" size="sm">Discard</Button>
						<Button size="sm" type="submit">Create</Button>
					</div>
				</div>
			</form>
		</main>
</div>

<Toaster />