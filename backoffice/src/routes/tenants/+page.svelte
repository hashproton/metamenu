<script lang="ts">
	import ListFilter from 'lucide-svelte/icons/list-filter';
	import ChevronLeft from 'lucide-svelte/icons/chevron-left';
	import ChevronRight from 'lucide-svelte/icons/chevron-right';

	import { Badge } from '$lib/components/ui/badge/index.js';
	import { Button } from '$lib/components/ui/button/index.js';
	import * as Card from '$lib/components/ui/card/index.js';
	import * as DropdownMenu from '$lib/components/ui/dropdown-menu/index.js';
	import * as Table from '$lib/components/ui/table/index.js';

	import * as Pagination from '$lib/components/ui/pagination/index.js';
	import { goto } from '$app/navigation';
	import { isApiError } from '$lib/clients';
	import { TenantStatus } from '$lib/clients/tenants_client.js';
	import { toast } from 'svelte-sonner';

	function mapTenantStatusColor(status: TenantStatus) {
		switch (status) {
			case TenantStatus.Inactive:
				return 'destructive';
			case TenantStatus.Active:
				return 'secondary';
			case TenantStatus.Demo:
				return 'default';
		}
	}

	let { data } = $props();

	$effect(() => {
		if (isApiError(data)) {
			setTimeout(() => {
				toast.error(data.errors[0].message, { duration: 2000 });
			}, 0);
		}
	});

	console.log(data);
</script>

{#if !isApiError(data)}
<main
	class="grid flex-1 items-start gap-4 p-4 sm:px-6 sm:py-0 md:gap-8 lg:grid-cols-3 xl:grid-cols-3"
>
	<div class="grid auto-rows-max items-start gap-4 md:gap-8 lg:col-span-2">
		<div class="flex justify-end gap-2">
			<DropdownMenu.Root>
				<DropdownMenu.Trigger asChild let:builder>
					<Button variant="outline" size="sm" class="h-7 gap-1 text-sm" builders={[builder]}>
						<ListFilter class="h-3.5 w-3.5" />
						<span class="sr-only sm:not-sr-only">Filter</span>
					</Button>
				</DropdownMenu.Trigger>
				<DropdownMenu.Content align="end">
					<DropdownMenu.Label>Filter by</DropdownMenu.Label>
					<DropdownMenu.Separator />
					<DropdownMenu.CheckboxItem checked>All</DropdownMenu.CheckboxItem>
					<DropdownMenu.CheckboxItem>Active</DropdownMenu.CheckboxItem>
					<DropdownMenu.CheckboxItem>Inactive</DropdownMenu.CheckboxItem>
				</DropdownMenu.Content>
			</DropdownMenu.Root>
			<a href="/tenants/create">
				<Button variant="default" size="sm">Add Tenant</Button>
			</a>
		</div>
		<Card.Root>
			<Card.Header class="px-7">
				<Card.Title>Tenants</Card.Title>
				<Card.Description>All businesses of metamenu.</Card.Description>
			</Card.Header>
			<Card.Content>
				{#if data.items && data.items.length === 0}
					<div class="flex h-32 items-center justify-center">
						<p
							class="-foreground
							text-muted"
						>
							No tenants found.
						</p>
					</div>
				{:else}
				<div class="h-[40ch]">
						<Table.Root>
							<Table.Header>
								<Table.Row>
									<Table.Head class="w-[50ch]">Name</Table.Head>
									<Table.Head class="hidden sm:table-cell">Status</Table.Head>
									<Table.Head class="text-right">Products</Table.Head>
								</Table.Row>
							</Table.Header>
							<Table.Body>
								{#each data.items! as tenant}
									<Table.Row>
										<Table.Cell>
											<div class="font-medium">
												<a href={`/tenants/${tenant.id}`}>{tenant.name}</a>
											</div>
											<div class="hidden text-sm text-muted-foreground md:inline">
												<a href={'notimplemented'} target="_blank">{'notimplemented'}</a>
											</div>
										</Table.Cell>
										<Table.Cell class="hidden sm:table-cell">
											<Badge class="text-xs" variant={mapTenantStatusColor(tenant.status)}>
												{TenantStatus[tenant.status]}
											</Badge>
										</Table.Cell>
										<Table.Cell class="text-right">32</Table.Cell>
									</Table.Row>
								{/each}
							</Table.Body>
						</Table.Root>
					</div>

					<Pagination.Root
						count={data.totalItems!}
						perPage={data.pageSize}
						let:pages
						let:currentPage
					>
						<Pagination.Content>
							<Pagination.Item>
								<Button
									disabled={!data.hasPreviousPage}
									variant="outline"
									class="flex justify-center border-0"
									onclick={() => goto(`?page=${data.pageNumber! - 1}`)}
								>
									<ChevronLeft strokeWidth={2} class="h-6 w-6" />
									<span class="hidden sm:block">Previous</span>
								</Button>
							</Pagination.Item>
							{#each pages as page (page.key)}
								{#if page.type === 'ellipsis'}
									<Pagination.Item>
										<Pagination.Ellipsis />
									</Pagination.Item>
								{:else}
									<Pagination.Item onclick={() => goto(`?page=${page.value}`)}>
										<Pagination.Link {page} isActive={data.pageNumber! === page.value}>
											{page.value}
										</Pagination.Link>
									</Pagination.Item>
								{/if}
							{/each}
							<Pagination.Item>
								<Button
									disabled={!data.hasNextPage}
									variant="outline"
									class="flex justify-center border-0"
									onclick={() => goto(`?page=${data.pageNumber! + 1}`)}
								>
									<span class="hidden sm:block">Next</span>
									<ChevronRight strokeWidth={2} class="h-6 w-6" />
								</Button>
							</Pagination.Item>
						</Pagination.Content>
					</Pagination.Root>
				{/if}
			</Card.Content>
		</Card.Root>
	</div>
	<div>
		<Card.Root class="overflow-hidden">
			<Card.Header class="flex flex-row items-start bg-muted/50">
				<div class="grid gap-0.5">
					<Card.Title class="group flex items-center gap-2 text-lg">
						Status
						<Button
							size="icon"
							variant="outline"
							class="h-6 w-6 opacity-0 transition-opacity group-hover:opacity-100"
						></Button>
					</Card.Title>
					<Card.Description>Date: November 23, 2023</Card.Description>
				</div>
			</Card.Header>
			<Card.Content class="p-6 text-sm">
				{#if !isApiError(data.info)}
					<div class="grid gap-3">
						<ul class="grid gap-3">
							<li class="flex items-center justify-between">
								<span class="text-muted-foreground">Active</span>
								<span>{data.info.active}</span>
							</li>
							<li class="flex items-center justify-between">
								<span class="text-muted-foreground">Inactive</span>
								<span>{data.info.inactive}</span>
							</li>
							<li class="flex items-center justify-between">
								<span class="text-muted-foreground">Demo</span>
								<span>{data.info.demo}</span>
							</li>
							<li class="flex items-center justify-between">
								<span class="text-muted-foreground">Total</span>
								<span>{data.info.total}</span>
							</li>
						</ul>
					</div>
				{/if}
			</Card.Content>
		</Card.Root>
	</div>
</main>
{/if}
